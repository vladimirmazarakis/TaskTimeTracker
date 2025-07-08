import z from 'zod'
import webClient from './webClient'
import { ref, type Ref } from 'vue'

enum TaskItemPriority {
  none = 0,
  low = 1,
  medium = 2,
  high = 3,
  urgent = 4,
}

type TaskItemDto = {
  id: number
  name: string
  description: string
  createdAt: Date
  updatedAt: Date
  isCompleted: boolean
  priority: number
}

type CreateTaskItemDto = {
  name: string
  description: string
  priority: number
}

type UpdateTaskItemDto = {
  id: number
  name: string
  description: string
  priority: number
}

async function getAllTaskItems() {
  const response = await webClient.get('/api/TaskItems')

  let tasks: TaskItemDto[] = []

  if (response.status === 200) {
    tasks = response.data.taskItems
    return tasks
  }

  return tasks
}

async function completeTask(taskId: number) {
  const response = await webClient.put('/api/TaskItems', {
    id: taskId,
    isCompleted: true,
  })

  if (response.status === 200) {
    return response.data
  }

  return
}

async function incompleteTask(taskId: number) {
  const response = await webClient.put('/api/TaskItems', {
    id: taskId,
    isCompleted: false,
  })

  if (response.status === 200) {
    return response.data
  }

  return
}

async function deleteTask(taskId: number) {
  const response = await webClient.delete(`/api/TaskItems/${taskId}`)
  return
}

const taskCreateUpdateSchema = z.object({
  name: z.coerce.string().min(1, 'Name is required'),
  description: z.coerce.string().optional(),
  priority: z.nativeEnum(TaskItemPriority).nullable().default(0),
})

async function createTask(createTaskItem: CreateTaskItemDto) {
  const response = await webClient.post(`/api/TaskItems`, createTaskItem)
  return
}

async function updateTask(updateTaskItem: UpdateTaskItemDto) {
  const response = await webClient.put(`/api/TaskItems`, updateTaskItem)
  return
}

const taskItemsInjectObjectDefaultValue: TaskItemsInjectObject = {
  taskItems: ref<TaskItemDto[]>([]),
  loading: ref(false),
  refreshTaskItems: () => {},
}

type TaskItemsInjectObject = {
  taskItems: Ref<TaskItemDto[] | undefined, TaskItemDto[] | undefined>
  loading: Ref<boolean, boolean>
  refreshTaskItems: () => void
}

async function getTaskSessionDuration(taskId: number) {
  const resp = await webClient.get(`/api/TaskSessions/${taskId}`)
  if (resp.status === 200) {
    return resp.data as number
  }
  return 0
}

async function getTotalTaskSessionDuration(taskId: number) {
  const resp = await webClient.get(`/api/TaskSessions/total/${taskId}`)
  if (resp.status === 200) {
    return resp.data as number
  }
  return 0
}

async function startTaskSession(taskId: number) {
  await webClient.post(`/api/TaskSessions/start/${taskId}`)
}

async function stopTaskSession(taskId: number) {
  await webClient.post(`/api/TaskSessions/stop/${taskId}`)
}

type TaskSessionDto = {
  startDate: Date
  endDate: Date
  totalDuration: number
}

async function getTaskSessionsHistory(taskId: number) {
  const resp = await webClient.get(`/api/TaskSessions/history/${taskId}`)
  if (resp.status === 200) {
    return resp.data.taskSessions as TaskSessionDto[]
  }
  return []
}

export {
  TaskItemPriority,
  type TaskItemDto,
  getAllTaskItems,
  completeTask,
  incompleteTask,
  deleteTask,
  taskCreateUpdateSchema,
  type CreateTaskItemDto,
  createTask,
  type TaskItemsInjectObject,
  taskItemsInjectObjectDefaultValue,
  updateTask,
  type UpdateTaskItemDto,
  getTaskSessionDuration,
  startTaskSession,
  stopTaskSession,
  getTotalTaskSessionDuration,
  getTaskSessionsHistory,
  type TaskSessionDto,
}
